using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Patterns;
using PureMVC.Interfaces;

class MediatorPrepCommand : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        Facade.RegisterMediator(new SceneMediator());
    }
}

