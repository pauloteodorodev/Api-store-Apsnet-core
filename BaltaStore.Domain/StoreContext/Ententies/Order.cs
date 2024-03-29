

using System;
using System.Collections.Generic;
using System.Linq;
using BaltaStore.Domain.StoreContext.Emuns;

namespace BaltaStore.Domain.StoreContext.Entities
{
    public class Order
    {
        private readonly IList<OrderItem> _items;
        private readonly IList<Delivery> _deliveries;

        public Order(Customer customer)
        {
            Customer = customer;
            CreateDate = DateTime.Now;
            Status = EOrdemStatus.Create;
            _items = new List<OrderItem>();
            _deliveries = new List<Delivery>();
        }

        public Customer Customer { get; private set; }
        public string Number { get; private set; }
        public DateTime CreateDate { get; private set; }
        public EOrdemStatus Status { get; private set; }

        public IReadOnlyCollection<OrderItem> Items => _items.ToArray();
        public IReadOnlyCollection<Delivery> Deliveries => _deliveries.ToArray();

        public void AddItem(OrderItem item)
        {
            _items.Add(item);
        }
        // public void AddDelivery(Delivery delivery)
        // {
        //     _deliveries.Add(delivery);
        // }
        public void Place()
        {
            Number = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper(); //Gerar numero pedido
        }
        public void Pay()
        {
            Status = EOrdemStatus.Paid;
        }
        public void Ship()
        {
            var deliveries = new List<Delivery>();
            deliveries.Add(new Delivery(DateTime.Now.AddDays(5)));
            var count = 1;


            foreach (var item in _items)
            {
                if (count == 5)
                {
                    count = 1;
                    deliveries.Add(new Delivery(DateTime.Now.AddDays(5)));
                }

                count++;
            }

            deliveries.ForEach(x => x.Ship());
            deliveries.ForEach(x => _deliveries.Add(x));

            var delivery = new Delivery(DateTime.Now.AddDays(5));
            _deliveries.Add(delivery);
        }

        public void Cancell()
        {
            Status = EOrdemStatus.Canceled;
            _deliveries.ToList().ForEach(x => x.Cancel());
        }

    }
}