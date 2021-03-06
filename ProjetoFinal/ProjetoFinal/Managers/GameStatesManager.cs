﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PerformanceUtility.GameDebugTools;

using ProjetoFinal.Managers;

namespace ProjetoFinal.GameStateEngine
{
    class GameStatesManager : DrawableGameComponent
    {
        List<GameState> states = new List<GameState>();
        //List<GameState> statesToUpdate = new List<GameState>();

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Game game;

        InputManager inputManager = InputManager.Instance;

        public bool TraceEnabled { get; set; }

        public GameStatesManager(Game game) : base(game) 
        {
            this.game = game;
        }

        public override void Initialize()
        {
            foreach (GameState state in states)
                state.Initialize(game, spriteFont);

            base.Initialize();

            //isInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>(@"fonts/SegoeUI");

            foreach (GameState state in states)
                state.LoadContent(content);
        }

        protected override void UnloadContent()
        {
            foreach (GameState state in states)
                state.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            inputManager.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            //screensToUpdate.Clear();

            //foreach (GameState state in states)
            //    statesToUpdate.Add(state);

            //bool otherScreenHasFocus = !Game.IsActive;
            //bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            //while (states.Count > 0)
            //{
                // Pop the topmost screen off the waiting list.
                GameState state = states[states.Count - 1];

                //states.RemoveAt(states.Count - 1);

                // Update the screen.
                state.Update(gameTime/*, otherScreenHasFocus, coveredByOtherScreen*/);

                /*if (state.ScreenState == ScreenState.TransitionOn || state.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        //screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    //if (!screen.IsPopup)
                    //    coveredByOtherScreen = true;
                }*/
            //}

            // Print debug trace?
            if (TraceEnabled)
                TraceScreens();
        }

        public override void Draw(GameTime gameTime)
        {
            //Matrix scaleMatrix = Matrix.CreateScale(0.8f);
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, scaleMatrix);
            spriteBatch.Begin();

            foreach (GameState state in states)
            {
                // TODO: Mudar os estados dos estados (Ainda não fazemos isso!)
                if (state.State == GameStateState.Hidden)
                    continue;

                state.Draw(gameTime, spriteBatch, spriteFont);
            }

            spriteBatch.End();
        }

        public void AddState(GameState state/*, PlayerIndex? controllingPlayer*/)
        {
            //screen.ControllingPlayer = controllingPlayer;
            //screen.IsExiting = false;

            // TODO: Refactor-me, maybe?
            if(states.Count > 0)
                states[states.Count - 1].State = GameStateState.Hidden;

            state.GameStatesManager = this;
            state.LoadContent(Game.Content);

            states.Add(state);
        }

        // TODO: Refactor-me, maybe?
        public void ResignState(GameState state)
        {
            state.UnloadContent();

            states.Remove(state);
            //screensToUpdate.Remove(screen);

            states[states.Count - 1].State = GameStateState.Active;
        }

        void TraceScreens()
        {
            List<string> statesNames = new List<string>();

            foreach (GameState state in states)
                statesNames.Add(state.GetType().Name);

            Console.WriteLine(string.Join(", ", statesNames.ToArray()));
        }

        public void ExitGame()
        {
            Game.Exit();
        }
    }
}
