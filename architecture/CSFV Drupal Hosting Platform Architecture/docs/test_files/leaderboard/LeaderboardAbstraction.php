<?php

require_once ('LeaderboardItem.php');

class LeaderboardAbstraction
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
