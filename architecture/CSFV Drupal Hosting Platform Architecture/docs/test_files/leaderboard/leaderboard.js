(function ($) {

  Drupal.behaviors.leaderboardModule = {
    attach: function (context, settings) {
      $.get('get_leaderboard_json', null, show_leaderboard);
    }
  };
  
  function show_leaderboard(response) {
    //Use the response to show the leaderboard
    //Response must be added to HTML table with id = leaderboardTable
  }

})(jQuery);