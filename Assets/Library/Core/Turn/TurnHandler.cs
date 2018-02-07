namespace NonaEngine {
    public static class TurnHandler {

        public static int turn { get; set; }
        public static bool firstBehaviour { get; set; }

        public static void TurnEnd() {
            if(firstBehaviour) {
                firstBehaviour = false;
            } else {
                turn++;
                firstBehaviour = true;
            }
        }

    }
}