﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	/// <summary>
	/// Represents a collection of gems.
	/// </summary>
	class PlayArea
	{
        private List<Gem> mGems;
		private Dictionary<GemType, int> mGemTypeToID;
		private List<List<Texture2D>> mGemTextures;
		private Random mRNG;

		private RuleChecker mRules;
		bool mCheckRulesNextUpdate;
		Gem mSwapGem1;
		Gem mSwapGem2;

		public PlayArea(List <List<Texture2D>> gemTextures)
		{
			mGemTextures = gemTextures;
			mRNG = new Random ();
			mGems = new List<Gem>(GlobalVars.GridDimensionX * GlobalVars.GridDimensionY);
			mGemTypeToID = new Dictionary<GemType, int> ();

			mGemTypeToID [GemType.Yellow] = 0;
			mGemTypeToID [GemType.White] = 1;
			mGemTypeToID [GemType.Blue] = 2;
			mGemTypeToID [GemType.Red] = 3;
			mGemTypeToID [GemType.Purple] = 4;
			mGemTypeToID [GemType.Orange] = 5;
			mGemTypeToID [GemType.Green] = 6;

			for (int x = 0; x < GlobalVars.GridDimensionX; ++x)
			{
				for (int y = 0; y < GlobalVars.GridDimensionY; ++y)
				{
					Gem newGem = GetNewGem ();
					newGem.MoveTo(new PlayAreaCoords(x, y));
					newGem.SetStartingLocation(new PlayAreaCoords(x, y - GlobalVars.GridDimensionY));
					mGems.Add(newGem);
				}
			}

			mRules = new RuleChecker();

			mSwapGem1 = null;
			mSwapGem2 = null;
		}

		private Gem GetNewGem()
		{
			int gemID = mRNG.Next(0, mGemTypeToID.Count);
			Gem gem = new Gem(mGemTypeToID.First(i => i.Value == gemID).Key, mGemTextures[gemID], this);
			return gem;
		}

		public void GemMoveAnimationCompleted()
		{
			mCheckRulesNextUpdate = true;
		}

		public void GemDisappearAnimationComplete(Gem gem)
		{
			int gemIndex = mGems.FindIndex(i => i == gem);
			PlayAreaCoords gemLoc = new PlayAreaCoords(gem.BoardLocation.X, gem.BoardLocation.Y);
			Gem newGem = GetNewGem();
			newGem.MoveTo(gemLoc);
			newGem.SetStartingLocation(new PlayAreaCoords(gemLoc.X, gemLoc.Y - GlobalVars.GridDimensionY));

			mGems[gemIndex] = newGem;
		}

		public List<Gem> Gems
		{
			get { return mGems; }
		}

		public PlayAreaCoords Size
		{
			get { return new PlayAreaCoords(GlobalVars.GridDimensionX, GlobalVars.GridDimensionY); }
		}

		public void Update(GameTime gameTime)
		{
			if (mCheckRulesNextUpdate)
			{
				if (!mRules.FindMatches(this))
				{
					//these can be null during startup
					if (mSwapGem1 != null && mSwapGem2 != null && GlobalVars.EnforceMoveMustResultInMatch)
						mSwapGem1.Swap(mSwapGem2); //if the move doesnt result in a match, move the gems back.
				}
				mSwapGem1 = null;
				mSwapGem2 = null;
				mCheckRulesNextUpdate = false;
			}

			for (int i = 0; i < mGems.Count; ++i)
			{
				mGems[i].Update (gameTime);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Gem gem in mGems)
			{
				gem.Draw (spriteBatch);
			}
		}

		public void MouseClicked (MouseEvent mouseEvent)
		{
			Gem clickedGem = mGems.Find (i => i.Contains (mouseEvent.MouseLocation));
			Gem selectedGem = mGems.Find (i => i.IsSelected);

			if (clickedGem == null)
			{
				Debug.WriteLine ("Did not find a gem at the mouse click.");
				return;
			}

			if (selectedGem != null)
			{
				if (selectedGem != clickedGem)
				{
					clickedGem.Swap(selectedGem);
					mSwapGem1 = selectedGem;
					mSwapGem2 = clickedGem;
				}
				else
				{
					selectedGem.IsSelected = false;
				}
			}
			else
			{
				clickedGem.IsSelected = true;
			}
		}

		public void MouseDragged(MouseEvent mouseEvent)
		{
			Gem fromGem = mGems.Find(i => i.Contains(mouseEvent.DragStart));
			Gem toGem = mGems.Find(i => i.Contains(mouseEvent.DragEnd));

			if (fromGem != null && toGem != null)
			{
				fromGem.Swap(toGem);
				mSwapGem1 = toGem;
				mSwapGem2 = fromGem;
			}
			else
			{
				Debug.WriteLine("Could not find two gems to drag...");
			}
		}
	}
}
