using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachineV2_StatePattern
{
    public interface IVendingMachine
    {
        List<Product> Products { get; }
        int CashInserted { get; }
        int ProductChosen { get; set; }

        void ChangeState(IState state);
        void MakeChoice(int productId);
        void InsertMoney(int amount);
        (Product, int) DispenseItem();
        int RefundMoney();
        void ResetMachine();
    }

    public interface IState
    {
        (Product, int) DoWork();
    }

    public class VendingMachine : IVendingMachine
    {
        private IState _state;
        public List<Product> Products { get; }
        public int CashInserted { get; private set; }

        public int ProductChosen { get; set; }

        public VendingMachine()
        {
            CashInserted = 0;
            Products = PopulateProducts();
            _state = new AwaitingChoice(this);
        }

        public void ChangeState(IState state)
        {
            _state = state;
        }

        public void MakeChoice(int productId)
        {
            ProductChosen = productId;
            _state.DoWork();
        }

        public void InsertMoney(int amount)
        {
            CashInserted += amount;
            _state.DoWork();
        }
        public (Product, int) DispenseItem()
        {
            return _state.DoWork();
        }

        public int RefundMoney()
        {
            _state = new Canceling(this);
            var (product, refund) = _state.DoWork();

            CashInserted = 0;

            return refund;
        }

        public void ResetMachine()
        {
            CashInserted = 0;
            ProductChosen = 0;
        }

        private static List<Product> PopulateProducts()
        {
            return new List<Product>()
            {
                new Product(1, "Guinness", 3),
                new Product(2, "Tayto", 2)
            };
        }
    }

    public class AwaitingChoice : IState
    {
        private readonly IVendingMachine _vendingMachine;
        public AwaitingChoice(IVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
            _vendingMachine.ResetMachine();
            Console.WriteLine($"Available products: {string.Join(",", _vendingMachine.Products.Select(x => $"Id: {x.Id}, Name: {x.Name}, Price: {x.Price}").ToArray())}");
        }

        public (Product, int) DoWork()
        {
            _vendingMachine.ChangeState(new AwaitingMoney(_vendingMachine));
            return (null, 0);
        }
    }

    public class AwaitingMoney : IState
    {
        private readonly IVendingMachine _vendingMachine;
        private readonly Product _productChosen;
        public AwaitingMoney(IVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
            _productChosen = _vendingMachine.Products.FirstOrDefault(x => x.Id == _vendingMachine.ProductChosen);
                  Console.WriteLine($"You have chosen {_productChosen.Name}, price is {_productChosen.Price}, please insert coins.");
        }


        public (Product, int) DoWork()
        {
            if (_vendingMachine.CashInserted < _productChosen.Price)
            {
                Console.WriteLine($"Price is {_productChosen.Price}, you have paid {_vendingMachine.CashInserted}, please insert more coins.");
            }
            else
            {
                _vendingMachine.ChangeState(new Dispensing(_vendingMachine));
            }

            return (null, 0);
        }
    }

    public class Dispensing : IState
    {
        private readonly IVendingMachine _vendingMachine;
        private readonly Product _productChosen;

        public Dispensing(IVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
            _productChosen = _vendingMachine.Products.FirstOrDefault(x => x.Id == _vendingMachine.ProductChosen);
            Console.WriteLine($"You have paid, dispensing {_productChosen.Name}, please wait...");
        }

        public (Product, int) DoWork()
        {
            var returningChange = _vendingMachine.CashInserted > _productChosen.Price
                ? _vendingMachine.CashInserted - _productChosen.Price
                : 0;
            Console.WriteLine($"Here is your product {_productChosen.Name}, returning change {returningChange}.");

            _vendingMachine.ChangeState(new AwaitingChoice(_vendingMachine));

            return (_productChosen, returningChange);
        }
    }

    public class Canceling : IState
    {
        private readonly IVendingMachine _vendingMachine;

        public Canceling(IVendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
            Console.WriteLine("Cancelling, please wait...");
        }

        public (Product, int) DoWork()
        {
            var refund = _vendingMachine.CashInserted;
            Console.WriteLine($"Here is your refund of {refund}...");

            _vendingMachine.ChangeState(new AwaitingChoice(_vendingMachine));

            return (null, refund);
        }
    }

    public class Product
    {
        public int Id { get; }
        public string Name { get; }

        public int Price { get; }

        public Product(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IVendingMachine vendingMachine = new VendingMachine();

            //Choose Product
            vendingMachine.MakeChoice(1);

            //Insert money
            vendingMachine.InsertMoney(1);

            //Not enough, insert more
            vendingMachine.InsertMoney(3);

            var (product, change) = vendingMachine.DispenseItem();
            Console.WriteLine($"Got my {product.Name}, change is {change}.");

            //Choose Product
            vendingMachine.MakeChoice(2);

            //Insert money
            vendingMachine.InsertMoney(1);

            //Ah, no coins left, I will cancel
            vendingMachine.RefundMoney();

            Console.ReadLine();
        }
    }
}