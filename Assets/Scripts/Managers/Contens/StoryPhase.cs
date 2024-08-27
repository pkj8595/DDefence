using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPhase : IPhase
{
    Data.StoryData _storyData;

    public void SetStoryData(Data.StoryData data)
    {
        if (Managers.Game.StoryStack.TryPop(out Data.StoryData storyData))
        {
            _storyData = storyData;
        }
        else
        {
            _storyData = Managers.Data.StoryDict[111999999];
        }
    }

    public void EnterPhase()
    {
        UIStoryData storyData = new UIStoryData
        {
            StoryDataNum = _storyData.tableNum
        };
        Managers.UI.ShowUI<UIStoryBook>(storyData);
    }

    public void EndPhase()
    {
        
    }

}

public class BattleReadyPhase : IPhase
{


    public void EnterPhase()
    {

    }

    public void EndPhase()
    {

    }

}

public class BattlePhase : IPhase
{
    Data.StoryData _storyData;

    public void EnterPhase()
    {

    }

    public void EndPhase()
    {

    }

}

