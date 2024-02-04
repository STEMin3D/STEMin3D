<!DOCTYPE html>
<html>
<head>
<title>Welcome to STEMin3D!</title>
<style>
    body {
        width: 35em;
        margin: 0 auto;
        font-family: Tahoma, Verdana, Arial, sans-serif;
    }
 summary::-webkit-details-marker {
 color: #00ACF3;
 font-size: 125%;
 margin-right: 2px;
}
summary:focus {
	outline-style: none;
}
article > details > summary {
	font-size: 28px;
	margin-top: 16px;
}
details > p {
	margin-left: 24px;
}
details details {
	margin-left: 36px;
}
details details summary {
	font-size: 16px;
}

</style>
</head>
<body>
<h1>Welcome to STEMin3D!</h1>
<p>If you see this page, the web engine (and PHP) in the GitHub is
working. This page will eventually serve the content of 
<a href="https://STEMin3D.net/">STEMin3D.net</a>.<br/>
</p>

<p><em>Thank you for setting up the page, Aiden!</em></p>
</body>
</html>
<hr/>
<details>
<summary>Click here to see the PHP information of the server</summary>
<p>
<?php
  phpinfo();
?>
</p>
</details>
