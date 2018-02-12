namespace NonaEngine {
    public static class TurnHandler {
        public static int turn { get; set; }
        public static TurnType turnType { get; set; }
        public enum TurnType {
            player1,
            player2
        }
    }
}