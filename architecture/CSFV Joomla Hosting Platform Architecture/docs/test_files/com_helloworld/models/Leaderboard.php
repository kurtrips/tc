<?php

/**
 * @version		$Id: hello.php 15 2009-11-02 18:37:15Z chdemko $
 * @package		Joomla16.Tutorials
 * @subpackage	Components
 * @copyright	Copyright (C) 2005 - 2010 Open Source Matters, Inc. All rights reserved.
 * @author		Christophe Demko
 * @link		http://joomlacode.org/gf/project/helloworld_1_6/
 * @license		License GNU General Public License version 2 or later
 */

// No direct access to this file
defined('_JEXEC') or die('Restricted access');

// import Joomla modelitem library
jimport('joomla.application.component.modelitem');

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

/**
 * Leaderboard Model
 */
class ModelLeaderboard extends JModelItem
{
	/**
	 * Get the message
	 * @return string The message to be displayed to the user
	 */
	public function getLeaderboard() 
	{
		return array(
			new LeaderboardItem('Sameena', 'Life', 29), 
			new LeaderboardItem('Jim', 'Life', 27) 
		);
	}
}
