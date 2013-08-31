<?php
$str = isset ($_GET['log']) ? $_GET['log'] : false ;
if ($str) {
	$ff = fopen ('logs.txt', 'a+') ;
        fputs ($ff,$str) ;
        fputs ($ff,' ') ;
	fclose ($ff) ;
}
?>