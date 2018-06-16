using AutoMapper;

namespace Weapsy.Cqrs.Events
{
    public class EventFactory : IEventFactory
    {
        public dynamic CreateConcreteEvent(object @event)
        {
            var type = @event.GetType();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap(type, type); });
            var mapper = config.CreateMapper();
            dynamic result = mapper.Map(@event, type, type);
            return result;
        }
    }
}