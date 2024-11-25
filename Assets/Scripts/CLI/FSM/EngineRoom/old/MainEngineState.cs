namespace CLI.FSM
{
    public class MainEngineState : State
    {
        public MainEngineState(StateController controller) : base(controller)
        {
            directories.Add("power", new EngineState(controller));
        }
    }
}

