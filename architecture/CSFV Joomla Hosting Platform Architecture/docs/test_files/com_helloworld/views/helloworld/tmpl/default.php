<?php

/**
 * @version		$Id: default.php 15 2009-11-02 18:37:15Z chdemko $
 * @package		Joomla16.Tutorials
 * @subpackage	Components
 * @copyright	Copyright (C) 2005 - 2010 Open Source Matters, Inc. All rights reserved.
 * @author		Christophe Demko
 * @link		http://joomlacode.org/gf/project/helloworld_1_6/
 * @license		License GNU General Public License version 2 or later
 */
// No direct access to this file
defined('_JEXEC') or die('Restricted access');
JHTML::_( 'behavior.mootools' );

$ajax = "
   window.addEvent('domready', function() {
        $('start_ajax').addEvent('click', function(e) {
            e.stop();    
            var url = 'index.php?option=com_helloworld&task=updateLeaderboard&format=json';
            var x = new Request({
                url: url, 
                method: 'post', 
		onSuccess: function(responseText){
		    document.getElementById('ajax_container').innerHTML = responseText;
		}
            }).send();
        });
    })
    ";
    
    $doc = & JFactory::getDocument();
    $doc->addScriptDeclaration( $ajax );
    
?>
<div>
	<h1><?php echo $this->msg; ?></h1>
	<div><a id="start_ajax" href="#">Click here</a> to start Ajax request</div>
	<div id="ajax_container">
	    
	</div>	
</div>	
