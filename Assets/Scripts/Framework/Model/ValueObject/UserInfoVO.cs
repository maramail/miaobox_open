using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UserInfoVO {

    public UserInfoVO()
    {
    }
    //test
    public UserInfoVO(int id, int gold, int diamond)
    {
        this.Gold = gold;
        this.Diamond = diamond;
        this.Id = id;
    }
    public int Id { get; set; }

    public string PlayerName { get; set; }

    public int Level { get; set; }

    public int Vip { get; set; }

    public int Exp { get; set; }

    public int Gold { get; set; }

    public int Diamond { get; set; }

    public string Blueprint { get ; set ;} 

    public int facility1 { get; set; }

    public int facility2 { get; set; }

    public int facility3 { get; set; }

    public int facility4 { get; set; }

}
