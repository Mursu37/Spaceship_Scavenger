namespace CLI.FSM
{
    public class MainTerminalState : State
    {
        public MainTerminalState(StateController controller) : base(controller)
        {
            directories.Add("MainTerminal", new DoorState(controller));
        }
    }
}
