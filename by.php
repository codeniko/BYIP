<?php

date_default_timezone_set('UTC');
$bytime= time() + (3 * 60 * 60);


if (isset($_GET['write']))
{

	$ip=$_SERVER['REMOTE_ADDR'];

	$file=fopen("bylastcheck.txt","w");
	fwrite($file, $bytime);
	fclose($file);


	$file=fopen("byinfo.txt","r");
	$fileString = date("F j, Y, G:i", $bytime) . " | " . $ip . "\n";
	$firstCheck = false;

	while(!feof($file))
	{
		$s = fgets($file);

		if ($firstCheck == false)
		{
			$firstCheck = true;
			$arre = explode(" | ", $s);
			if (trim($arre[1]) === $ip)
				die("IP did not change");
		}

		$fileString = $fileString.$s;
	}

	fclose($file);

	$file=fopen("byinfo.txt","w");
	fwrite($file, $fileString);
	fclose($file);
	die("Recorded new IP");

} else 
{
	$file=fopen("bylastcheck.txt","r");
	echo "Last Check: ".date("F j, Y, G:i", fgets($file))."<br /><br />";
	fclose($file);

	$file=fopen("byinfo.txt","r");
	while(!feof($file))
	{
		echo fgets($file). "<br />";
	}
	fclose($file);
}

?>
