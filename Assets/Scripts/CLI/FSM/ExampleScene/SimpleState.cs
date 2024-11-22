using UnityEngine;

namespace CLI.FSM.ExampleScene
{
    public class SimpleState : State
    {
        public SimpleState(StateController controller) : base(controller)
        {
            directories.Add("next", new SimpleStateDeeper(controller));
            commands.Add("helloworld");
            commands.Add("polish");
        }

        public override void Interpret(string command)
        {
            switch (command)
            {
                case "helloworld":
                    stateController.ChangeText("Hello world");
                    return;
                case "polish":
                    stateController.ChangeText("We need to polish our game!");
                    return;
            }
            base.Interpret(command);
        }
    }
}
