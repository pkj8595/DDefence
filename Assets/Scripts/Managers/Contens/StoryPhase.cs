using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPhase : IPhase
{
    public void EnterPhase()
    {
        Data.StoryData storyData;
        if (!Managers.Game.StoryStack.IsEmpty())
        {
            storyData = Managers.Game.StoryStack.Dequeue();
        }
        else
        {
            storyData = Managers.Data.StoryDict[111999999];
        }

        UIStoryData uiStoryData = new UIStoryData
        {
            StoryDataNum = storyData.tableNum
        };
        Managers.UI.ShowUI<UIStoryBook>(uiStoryData);
    }

    public void EndPhase()
    {
        
    }

}

public class BattleReadyPhase : IPhase
{
    public void EnterPhase()
    {
        UIMain uiMain = Managers.UI.ShowUI<UIMain>() as UIMain;
        uiMain.ShowBtnNextPhase();

        Managers.Game.RunReadyWave();
    }

    public void EndPhase()
    {

    }

}

public class BattlePhase : IPhase
{
    public void EnterPhase()
    {
        Managers.Game.RunBattlePhase();
    }

    
    public void EndPhase()
    {
        UIMain uiMain = Managers.UI.ShowUI<UIMain>() as UIMain;
        uiMain.ShowBtnNextPhase();
    }

}

