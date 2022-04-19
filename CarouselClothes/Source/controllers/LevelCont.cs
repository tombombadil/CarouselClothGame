using Ruckcat;

public class LevelCont : HyperLevelCont
{
    public override void Init()
    {
        base.Init();
    }

    public override void StartGame()
    {
        base.StartGame();

    }

    public override void EndLevel(GameResult levelResult)
    {
        base.EndLevel(levelResult);

        if (levelResult == GameResult.SUCCEED)
        {
            SetLevelScore(0);
        }
    }
}