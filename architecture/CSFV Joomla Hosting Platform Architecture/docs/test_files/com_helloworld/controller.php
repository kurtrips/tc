<?php

/**
 * @version		$Id: controller.php 15 2009-11-02 18:37:15Z chdemko $
 * @package		Joomla16.Tutorials
 * @subpackage	Components
 * @copyright	Copyright (C) 2005 - 2010 Open Source Matters, Inc. All rights reserved.
 * @author		Christophe Demko
 * @link		http://joomlacode.org/gf/project/helloworld_1_6/
 * @license		License GNU General Public License version 2 or later
 */

// No direct access to this file
defined('_JEXEC') or die('Restricted access');

// import Joomla controller library
jimport('joomla.application.component.controller');



/**
 * Hello World Component Controller
 */
class HelloWorldController extends JController
{
	function updateLeaderboard() {
		// Get the model.
		$model = $this->getModel('Leaderboard', 'Model');

		// Get the data
		$data = $model->getLeaderboard();

			// Check for errors.
		if ($model->getError()) {
		    //Write some helpful message to json
		    //It should be a json object with just one key called errorMessage
		} 
        
		echo json_encode($data);
	}
}
