<?php

class LeaderboardItem
{
    public $player;
    public $game;
    public $score;
    
    function LeaderboardItem($player, $game, $score) {
        $this->player = $player;
        $this->game = $game;
        $this->score = $score;
   }
}
