public class EmployeeInfoVO
{
    public EmployeeInfoVO(int id, string name, int lv, int evo, int iq, int atk, int react, int skill,string about,int hireprice)
    {
        this.Id = id;
        this.Name = name;
        this.Level = lv;
        this.Evo = evo;
        this.Iq = iq;
        this.Atk = atk;
        this.React = react;
        this.Skill = skill;
        this.About = about;
        this.HirePrice = hireprice;

    }

    public int Id { set; get; }

    public string Name { set; get; }

    public int Level { set; get; }

    public int Evo { set; get; }

    public int Iq { set; get; }

    public int Atk { set; get; }

    public int React { set; get; }

    public int Skill { set; get; }

    public string About { get; set; }

    public int HirePrice { get; set; }

}