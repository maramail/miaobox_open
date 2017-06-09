/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:20
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\macrocommands\startupcommand.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\controller\macrocommands
	file base:	startupcommand
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	启动Command
*********************************************************************/
using UnityEngine;
using System.Collections;
using PureMVC.Patterns;

public class StartupCommand : MacroCommand
{
    protected override void InitializeMacroCommand()
    {
        AddSubCommand(typeof(ReadDataBaseCommand));
        AddSubCommand(typeof(ModelPrepCommand));
        AddSubCommand(typeof(MediatorPrepCommand));
        AddSubCommand(typeof(InitLoginMenuViewCommand));
    }
}