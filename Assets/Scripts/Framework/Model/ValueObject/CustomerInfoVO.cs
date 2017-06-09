public class CustomerInfoVO
{
    public CustomerInfoVO()
    { }

    public CustomerInfoVO(int id,int money)
    {
        Id = id;
        Money = money;
    }

    public int Id { get; set; }
    public int Money { get; set; }
}

