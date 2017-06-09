using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Patterns;
using PureMVC.Interfaces;



class InitBattleCommand: SimpleCommand
{
    public override void Execute(INotification notification)
    {
        UIBaseBehaviour<BattleMediator>.CreateUI<BattleView>();
    }


    }
 
