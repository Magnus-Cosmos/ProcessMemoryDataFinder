﻿using System;
using System.Collections.Generic;
using System.Linq;
using OsuMemoryDataProvider.OsuMemoryModels.Abstract;
using ProcessMemoryDataFinder.Structured;

namespace OsuMemoryDataProvider.OsuMemoryModels.Direct
{
    [MemoryAddress("[[CurrentRuleset]+0x74]+0x24")]
    public class LeaderBoard
    {
        public LeaderBoard()
        {
            //single player: top50 + player top score + current score
            RawPlayers = Enumerable.Range(0, 52).Select(x => new MultiplayerPlayer()).ToList();
        }

        [MemoryAddress("")]
        private int? RawHasLeaderboard { get; set; }
        public bool HasLeaderBoard => RawHasLeaderboard.HasValue && RawHasLeaderboard != 0;
        private MainPlayer _mainPlayer = new MainPlayer();

        [MemoryAddress("[[]+0x10]")]
        public MainPlayer MainPlayer
        {
            get => HasLeaderBoard ? _mainPlayer : null;
            set => _mainPlayer = value;
        }

        private int? _amountOfPlayers;

        [MemoryAddress("[[]+0x4]+0xC")]
        public int? AmountOfPlayers
        {
            get => _amountOfPlayers;
            set
            {
                _amountOfPlayers = value;
                if (value.HasValue && value.Value > 0)
                    Players = _players.GetRange(0, value.Value);
                else
                    Players.Clear();
            }
        }
        private List<MultiplayerPlayer> _players;
        [MemoryAddress("[]+0x4")]
        private List<MultiplayerPlayer> RawPlayers
        {
            //toggle reading of players depending on HasLeaderboard value
            get => HasLeaderBoard ? _players : null;
            set
            {
                _players = value;
            }
        }
        public List<MultiplayerPlayer> Players { get; private set; } = new List<MultiplayerPlayer>();
    }
}