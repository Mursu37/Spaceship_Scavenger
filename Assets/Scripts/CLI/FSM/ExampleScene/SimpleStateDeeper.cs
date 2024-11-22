namespace CLI.FSM.ExampleScene
{
    public class SimpleStateDeeper : State
    {
        public SimpleStateDeeper(StateController controller) : base(controller)
        {
        }

        public override void OnEnter()
        {
            stateController.ChangeFlavourText("Hello here is information about this directory");
        }
    }
}
