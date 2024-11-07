namespace CLI.FSM
{
    public class MainDriveState : State
    {
        public MainDriveState(StateController controller) : base(controller)
        {
            directories.Add("doors" ,new DoorState(controller));
        }
    }
}
