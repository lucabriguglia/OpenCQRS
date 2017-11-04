using AutoMapper;

namespace Weapsy.Mediator.Events
{
    public static class EventFactory
    {
        public static dynamic CreateConcreteEvent(object @event)
        {
            var type = @event.GetType();
            var config = new MapperConfiguration(cfg => { cfg.CreateMap(type, type); });
            var mapper = config.CreateMapper();
            dynamic result = mapper.Map(@event, type, type);
            return result;
        }
    }
}
