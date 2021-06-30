using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class WinEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameQuitEvent : SDD.Events.Event
{
}
public class GameStatisticsChangedEvent : SDD.Events.Event
{
    public float eNLives { get; set; }
}
public class GameObjectiveChangedEvent : SDD.Events.Event
{
    public int eNObjective { get; set; }
}
#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}
public class QuitButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region Player actions

public class PlayerHasAttackedEvent : SDD.Events.Event
{
}
public class PlayerHasBeenHitEvent : SDD.Events.Event
{
    public float eDamages { get; set; }
}
public class EnemyHasBeenHitEvent : SDD.Events.Event
{
    public float eDamages { get; set; }
    public GameObject eEnemy { get; set; }

}
public class Player2HasSummonedEnemyEvent : SDD.Events.Event
{
    public Transform eSpawnPosition { get; set; }
    public GameObject eEnemyType { get; set; }

}
public class PlayerHasKilledEnemyEvent : SDD.Events.Event
{
}

public class PlayerHasBeenHitAudioEvent : SDD.Events.Event
{

}

public class PlayerHasHitAudioEvent : SDD.Events.Event
{

}

public class PlayerHasMissHitAudioEvent : SDD.Events.Event
{

}

public class PlayerWalkingAudioEvent : SDD.Events.Event
{

}
public class PlayerStoppedWalkingAudioEvent : SDD.Events.Event
{

}


#endregion
