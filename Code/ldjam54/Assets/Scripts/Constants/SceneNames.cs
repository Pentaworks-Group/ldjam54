using System;
using System.Collections.Generic;

namespace Assets.Scripts.Constants
{
    public class SceneNames
    {
        public const String Credits = "CreditsScene";
        public const String GameOver = "GameOverScene";
        public const String MainMenu = "MainMenuScene";
        public const String Options = "OptionsScene";
        public const String SavedGames = "SavedGamesScene";
        public const String GameModes = "GameModesScene";
        public const String Space = "SpaceScene";

        public static List<String> scenes = new() { MainMenu, SavedGames, Options, Credits, Space, GameModes, GameOver };
        public static List<String> scenesDevelopment = new() {  };
    }
}