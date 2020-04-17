(function () {

    gameVariables();

    //===
    // Test
    //document.getElementById("test1").innerHTML = img.naturalHeight;
    //===

    if (hexmapStyle == "pointy") {
    }
    else if (hexmapStyle == "flat") {
        gameVariables_f();

        if (canvas.getContext) {
            ctx.imageSmoothingEnabled = false;

            ctx.fillStyle = "#CCCCCC";
            ctx.strokeStyle = "#000000";
            ctx.lineWidth = 4;

            //drawBoard_f(ctx, boardWidth, boardHeight);
            drawTextureBoard(ctx, img, boardWidth, boardHeight);

            canvas.addEventListener("mousemove", function (eventInfo) {
                var x,
                    y,
                    hexX,
                    hexY;

                x = eventInfo.offsetX || eventInfo.layerX;
                y = eventInfo.offsetY || eventInfo.layerY;

                x = (x - offsetX) / scaleX;
                y = (y - offsetY) / scaleY;

                hexX = Math.floor(x / (1.5 * sideLength));
                hexY = Math.floor((y - (hexX % 2) * hexInnerRadius) / hexRectangleHeight);

                clearBoard();
                //drawBoard_f(ctx, boardWidth, boardHeight);
                drawTextureBoard(ctx, img, boardWidth, boardHeight);

                //===
                // Compare original Hexagon(hexX, hexY) with its neighbors.
                // The Hexagon with the shortest distance to the cursor is the correct one.
                var p = offToPixel_f(hexX, hexY);
                var d = distance(p[0], p[1], x, y);

                var n3 = offNeighbor_f(hexX, hexY, 3);
                var p3 = offToPixel_f(n3[0], n3[1]);
                var d3 = distance(p3[0], p3[1], x, y);

                var n4 = offNeighbor_f(hexX, hexY, 4);
                var p4 = offToPixel_f(n4[0], n4[1]);
                var d4 = distance(p4[0], p4[1], x, y)

                if (d3 < d && d3 < d4) {
                    hexX = n3[0];
                    hexY = n3[1];
                }
                else if (d4 < d) {
                    hexX = n4[0];
                    hexY = n4[1];
                }
                //===




                // Check if the mouse's coords are on the board
                if (hexX >= 0 && hexX < boardWidth && hexY >= 0 && hexY < boardHeight) {
                    markedTileX = hexX;
                    markedTileY = hexY;

                    //drawHexagon_f(ctx, hexX, hexY, false);
                }
                else {
                    markedTileX = -1;
                    markedTileY = -1;
                }
            });

            canvas.addEventListener("wheel", function (eventInfo) {
                var scrollDirection = eventInfo.deltaY;
                scaleX = scaleX * (1 - scrollDirection / 1000);
                scaleY = scaleY * (1 - scrollDirection / 1000);

                ctx.scale(1 - scrollDirection / 1000, 1 - scrollDirection / 1000);
                clearBoard();
                drawTextureBoard(ctx, img, boardWidth, boardHeight);
                console.log(scaleX, scaleY);
                document.getElementById("test1").innerHTML = canvas.height;
            });
        }
    }


    //=====
    // Functions:

    function clearBoard() {
        ctx.save();
        ctx.setTransform(1, 0, 0, 1, 0, 0);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.restore();
    }

    function distance(x1, y1, x2, y2) {
        return Math.round(Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2)))
    }

    function drawTexture(image, col, row, textureIndex, marked) {
        pixelCol = col * 1.5 * sideLength;
        pixelRow = (row - 1) * hexRectangleHeight + ((col % 2) * hexInnerRadius);

        var imgHeight = hexRectangleHeight * 2 + (1.0 / 15.0 * hexRectangleHeight);
        var imgWidth = hexRectangleWidth;
        var tileX = textureIndex * 128 / 4;

        if (marked == true) {
            //drawHexagon_f(ctx, col, row + 1, false);
            ctx.beginPath();
            ctx.moveTo(pixelCol + 0.5 * sideLength, pixelRow + hexRectangleHeight);
            ctx.lineTo(pixelCol + 1.5 * sideLength, pixelRow + hexRectangleHeight);
            ctx.lineTo(pixelCol + hexRectangleWidth, pixelRow + hexRectangleHeight + hexInnerRadius);
            ctx.lineTo(pixelCol + 1.5 * sideLength, pixelRow + hexRectangleHeight + hexRectangleHeight);
            ctx.lineTo(pixelCol + 0.5 * sideLength, pixelRow + hexRectangleHeight + hexRectangleHeight);
            ctx.lineTo(pixelCol, pixelRow + hexRectangleHeight + hexInnerRadius);
            ctx.closePath();
            ctx.stroke();


            ctx.globalAlpha = 0.5;
        }
        ctx.drawImage(image, tileX, 0, 128 / 4, 232 / 4, pixelCol, pixelRow, imgWidth, imgHeight);

        ctx.globalAlpha = 1;
        //console.log("Row:" + row + ", Column: " + col + ", ImageStartX: " + pixelCol + ", ImageStartY: " + pixelRow);
    }

    function drawTextureBoard(canvasContext, image, width, height) {
        for (var i = 0; i < height; i++) {

            for (var j = 0; j < width; j += 2) {
                drawTexture(image, j, i, textures[j][i], (i == markedTileY && j == markedTileX));
            }
            for (var j = 1; j < width; j += 2) {
                drawTexture(image, j, i, textures[j][i], (i == markedTileY && j == markedTileX));
            }
        }
    }
    //=====


    //=====
    // Functions for flat top hexagons:
    function drawBoard_f(canvasContext, width, height) {
        for (var i = 0; i < width; i++) {
            for (var j = 0; j < height; j++) {
                drawHexagon_f(canvasContext, i, j, false);
            }
        }
    }

    function drawColoredHex_f(canvasContext, col, row, colorIndex) {
        switch (colorIndex) {
            case 0:
                canvasContext.fillStyle = "#95e639";
                break;
            case 1:
                canvasContext.fillStyle = "#42d4b9";
                break;
            case 2:
                canvasContext.fillStyle = "#a342d4";
                break;
            case 3:
                canvasContext.fillStyle = "#d49242";
                break;
            default:
                canvasContext.fillStyle = "#bab9b8";
                break
        }

        drawHexagon_f(canvasContext, col, row, true);
    }

    function drawHexagon_f(canvasContext, col, row, fill) {
        var fill = fill || false;

        pixelRow = row * hexRectangleHeight + ((col % 2) * hexInnerRadius);
        pixelCol = col * 1.5 * sideLength;

        canvasContext.beginPath();
        canvasContext.moveTo(pixelCol + 0.5 * sideLength, pixelRow);
        canvasContext.lineTo(pixelCol + 1.5 * sideLength, pixelRow);
        canvasContext.lineTo(pixelCol + hexRectangleWidth, pixelRow + hexInnerRadius);
        canvasContext.lineTo(pixelCol + 1.5 * sideLength, pixelRow + hexRectangleHeight);
        canvasContext.lineTo(pixelCol + 0.5 * sideLength, pixelRow + hexRectangleHeight);
        canvasContext.lineTo(pixelCol, pixelRow + hexInnerRadius);
        canvasContext.closePath();

        if (fill) {
            canvasContext.fill();
        } else {
            canvasContext.stroke();
        }
    }

    function offToCube_f(col, row) {
        var x = col;
        var z = row - (col - (col & 1)) / 2;
        var y = -x - z;
        return [x, y, z];
    }

    function cubeToAxial_f(x, y, z) {
        var q = x;
        var r = z;
        return [q, r];
    }

    function offToAxial_f(col, row) {
        var cube = offToCube_f(col, row);
        var axial = cubeToAxial_f(cube[0], cube[1], cube[2]);
        return [axial[0], axial[1]];
    }

    function axialToPixel_f(q, r) {
        var x = sideLength * (3. / 2 * q);
        var y = sideLength * (Math.sqrt(3) / 2 * q + Math.sqrt(3) * r);

        //set pixel coordinates to the middle of the hexagon
        x = Math.round(x + sideLength);
        y = Math.round(y + hexInnerRadius);
        return [x, y];
    }

    function offToPixel_f(col, row) {
        var axial = offToAxial_f(col, row);
        var pixel = axialToPixel_f(axial[0], axial[1]);
        return [pixel[0], pixel[1]];
    }

    function offNeighbor_f(col, row, direction) {
        var parity = col & 1;
        var dir = offDirections[parity][direction];
        return [col + dir[0], row + dir[1]];
    }
    //=====
})();


var isDown = false; // whether mouse is pressed
var startCoords = []; // 'grab' coordinates when pressing mouse
var last = [0, 0]; // previous coordinates of mouse release

canvas.onmousedown = function (e) {
    isDown = true;

    startCoords = [
        e.offsetX - last[0], // set start coordinates
        e.offsetY - last[1]
    ];
};

canvas.onmouseup = function (e) {
    isDown = false;

    last = [
        e.offsetX - startCoords[0], // set last coordinates
        e.offsetY - startCoords[1]
    ];
};

canvas.onmousemove = function (e) {
    if (!isDown) return; // don't pan if mouse is not pressed

    var x = e.offsetX;
    var y = e.offsetY;

    // set the canvas' transformation matrix by setting the amount of movement:
    // 1  0  dx
    // 0  1  dy
    // 0  0  1

    ctx.setTransform(scaleX, 0, 0, scaleY,
        x - startCoords[0], y - startCoords[1]);

    offsetX = x - startCoords[0];
    offsetY = y - startCoords[1];


}