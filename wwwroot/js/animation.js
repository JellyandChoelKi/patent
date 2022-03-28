var canvas = document.getElementById('mycanvas');
var ctx = canvas.getContext('2d');

var p = { x: 25, y: 25 };
var velo = 3,
	corner = 50,
	rad = 10;
var ball = { x: p.x, y: p.y };
var moveX = Math.cos(Math.PI / 180 * corner) * velo;
var moveY = Math.sin(Math.PI / 180 * corner) * velo;

function DrawMe() {
	ctx.clearRect(0, 0, 500, 400);

	if (ball.x > canvas.width - rad || ball.x < rad) moveX = -moveX;
	if (ball.y > canvas.height - rad || ball.y < rad) moveY = -moveY;

	ball.x += moveX;
	ball.y += moveY;

	ctx.beginPath();
	ctx.fillStyle = '#CD1F48';
	ctx.arc(ball.x, ball.y, rad, 0, Math.PI * 2, false);
	ctx.fill();
	ctx.closePath();
}
setInterval(DrawMe, 10);