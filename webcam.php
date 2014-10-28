<?php

$data=$_POST['data'];

   $contents_split = explode(',', $data);
   $encoded = $contents_split[count($contents_split)-1];
   $decoded = "";
   for ($i=0; $i < ceil(strlen($encoded)/256); $i++) {
      $decoded = $decoded . base64_decode(substr($encoded,$i*256,256)); 
   }
   $data = $decoded; 
$fp = fopen("img.png", "w");
fwrite($fp, $data);
fclose($fp); 
?>