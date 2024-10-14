namespace MDesingPattern
{
    namespace MMediator
    {
        public interface IMediator<Sender,Message>   where Sender  : class 
                                                     where Message : class
        {
           public void Notify(Sender sender,Message message);
        }

        public interface IMediatable<Sender,Message> where Sender  : class
                                                     where Message : class
        {
            public void SetMediator(IMediator<Sender,Message> mediator) ;
        }

    }

}