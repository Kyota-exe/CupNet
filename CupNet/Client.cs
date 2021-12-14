namespace CupNet
{
    public class Client
    {
        public int id;
        public TCP tcp;

        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }
    }
}