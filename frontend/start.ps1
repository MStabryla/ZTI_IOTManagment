$env:HTTPS = "true";
$env:SSL_CRT_FILE='.\loc.crt';
$env:SSL_KEY_FILE='.\loc.key';
npm run start;