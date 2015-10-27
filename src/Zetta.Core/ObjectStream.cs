using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zetta.Core {
    public class ObjectStream {
        private DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0);

        private IList<Action<StreamMessage>> _onDataSubscribers = new List<Action<StreamMessage>>();

        public void Subscribe(Action<StreamMessage> subscriber) {
            _onDataSubscribers.Add(subscriber);
        }

        public void Publish(string json) {
            var jsonMessage = JObject.Parse(json);

            var message = new StreamMessage();
            message.Topic = jsonMessage.Property("topic").Value.ToString();
            message.Date = _epoch.AddMilliseconds((double)jsonMessage.Property("timestamp").Value);
            message.Data = jsonMessage.Property("data").Value;

            _onDataSubscribers.ToList().ForEach((subscriber) => {
                subscriber.Invoke(message);
            });
        }
    }
}
