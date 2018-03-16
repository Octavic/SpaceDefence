//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GameOverScreen.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;
    using UI.Graph;

    /// <summary>
    /// Rendered when the game is over, and the win/lose text is shown with graphs with score
    /// </summary>
    public class GameOverScreen : MonoBehaviour
    {
        /// <summary>
        /// Text to place the final score
        /// </summary>
        public Text FinalScoreText;

        /// <summary>
        /// THe graph game objects
        /// </summary>
        public LineGraph CostLineGraph;
        public LineGraph IncomeLineGraph;
        public BarGraph HealthBarGraph;

        /// <summary>
        /// Called when the game is over
        /// </summary>
        /// <param name="isWin">If the player won the game</param>
        /// <param name="enemyReachEndPenalty">The amount of total  health</param>
        /// <param name="incomeData">A list of incomes over time</param>
        /// <param name="costData">A list of costs over time</param>
        /// <param name="healthData">A list of health over time</param>
        public void OnGameOver(bool isWin, float enemyReachEndPenalty, IList<float> incomeData, IList<float> costData, IList<float> healthData)
        {
            this.IncomeLineGraph.DrawGraph(incomeData);
            this.CostLineGraph.DrawGraph(costData);
            this.HealthBarGraph.DrawGraph(healthData, 0);
            var finalScore = incomeData.Last() - costData.Last() - enemyReachEndPenalty;
            this.FinalScoreText.text = finalScore.ToString();
        }

        public void OnRestart()
        {
            this.gameObject.SetActive(false);
        }
    }
}
