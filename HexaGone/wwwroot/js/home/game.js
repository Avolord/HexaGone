(function () {

    // setting the game variables from the Game.cshtml file
    gameVariables();

    if (canvas.getContext) {

        // ensure that the textures stay high-quality when zooming in on the map
        ctx.imageSmoothingEnabled = false;

        // set the style when drawing figures inside of the canvas
        ctx.fillStyle = "#CCCCCC";
        ctx.strokeStyle = "#000000";
        ctx.lineWidth = 4;

        // draw the map
        drawMap();

        // handle the event when hovering over the map with the mouse
        canvas.addEventListener("mousemove", function (eventInfo) {

            // Position of the mouse cursor on the canvas
            var mousePosX;
            var mousePosY;

            // offset-coordinate of the current tile the mouse is hovering over
            var hexX;
            var hexY;

            // setting the mouse position variable to the current mouse position
            mousePosX = eventInfo.offsetX || eventInfo.layerX;
            mousePosY = eventInfo.offsetY || eventInfo.layerY;

            // correcting the mouse position with the offset of the map (from dragging the map around) and the scale of the map (from zooming in and out of the map)
            mousePosX = (mousePosX - offsetX) / scaleX;
            mousePosY = (mousePosY - offsetY) / scaleY;

            // setting the currently hovered-over tile temporarily (inaccurate, but necessary for further corrections)
            hexX = Math.floor(mousePosX / (1.5 * sideLength));
            hexY = Math.floor((mousePosY - (hexX % 2) * hexInnerRadius) / hexRectangleHeight);

            // Correcting the currently hovered-over tile for more accuracy:
            // Converting the offset-coordinate of the temporary hovered-over tile into a pixel location in the center of the hexagon and measuring the distance to the mouse position
            var p = offsetToPixelCoords(hexX, hexY);
            var d = distance(p[0], p[1], mousePosX, mousePosY);

            // finding the third neighbor, converting the offset-coordinate into a pixel location and measuring the distance to the mouse position
            var n3 = getNeighborCoords(hexX, hexY, 3);
            var p3 = offsetToPixelCoords(n3[0], n3[1]);
            var d3 = distance(p3[0], p3[1], mousePosX, mousePosY);

            // finding the fourth neighbor, converting the offset-coordinate into a pixel location and measuring the distance to the mouse position
            var n4 = getNeighborCoords(hexX, hexY, 4);
            var p4 = offsetToPixelCoords(n4[0], n4[1]);
            var d4 = distance(p4[0], p4[1], mousePosX, mousePosY)

            // correcting the temporary hovered-over tile (hexX, hexY) to the neighbor with the closest distance to the the mouse position (if necessary)
            if (d3 < d && d3 < d4) {
                hexX = n3[0];
                hexY = n3[1];
            }
            else if (d4 < d) {
                hexX = n4[0];
                hexY = n4[1];
            }

            // Check if the mouse's coords are on the map
            if (hexX >= 0 && hexX < mapWidth && hexY >= 0 && hexY < mapHeight) {
                // setting the marked tile to the currently hovered-over tile
                markedTileX = hexX;
                markedTileY = hexY;
            }
            else {
                // set the marked tile to a location outside of the map, when the mouse's coords are not on the map
                markedTileX = -1;
                markedTileY = -1;
            }

            // clear the current map and redraw it correctly
            clearMap();
            drawMap();
        });

        // handle the event of scrolling the mousewheel when over the canvas
        canvas.addEventListener("wheel", function (eventInfo) {

            // temporary variable for the direction the mousewheel is scrolled
            var scrollDirection = eventInfo.deltaY;

            // setting the global scale variable to a new scale depending on the mousewheel scroll
            scaleX = scaleX * (1 -scrollDirection / 1000);
            scaleY = scaleY * (1 - scrollDirection / 1000);

            // setting the new scale for the canvas context
            ctx.scale(1 - scrollDirection / 1000, 1 - scrollDirection / 1000);

            // clear the current map and redraw it correctly
            clearMap();
            drawMap();
        });
    }

    // clear the canvas
    function clearMap() {

        // save the current transform of the map
        ctx.save();

        // setting the transform to default settings
        ctx.setTransform(1, 0, 0, 1, 0, 0);

        // clear the canvas
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        // restore the saved transform of the map
        ctx.restore();
    }

    // calculate the distance between two points
    function distance(x1, y1, x2, y2) {
        return Math.round(Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2)))
    }

    // draw the current map
    function drawMap() {

        // iterate through the rows of the map
        for (var i = 0; i < mapHeight; i++) {

            // iterate through the even columns of the map
            for (var j = 0; j < mapWidth; j += 2) {
                drawTile(j, i, mapData[j][i], (i == markedTileY && j == markedTileX));
            }

            // iterate through the odd columns of the map
            for (var j = 1; j < mapWidth; j += 2) {
                drawTile(j, i, mapData[j][i], (i == markedTileY && j == markedTileX));
            }
        }
    }

    // draw a tile
    function drawTile(col, row, textureIndex, marked) {

        // get the pixel position of the current tile
        var pixelCol = col * 1.5 * sideLength;
        var pixelRow = (row - 1) * hexRectangleHeight + ((col % 2) * hexInnerRadius);

        // the height of the drawn image
        var imgHeight = hexRectangleHeight * 2 + (1.0 / 15.0 * hexRectangleHeight);
        // the width of the drawn image
        var imgWidth = hexRectangleWidth;

        // position of the wanted texture in the tilesheet image
        var tileX = textureIndex * 128 / 4;

        if (marked == true) {
            strokeHexagon(pixelCol, pixelRow);
            ctx.globalAlpha = 0.5;
        }
        ctx.drawImage(tilesheetImage, tileX, 0, 128 / 4, 232 / 4, pixelCol, pixelRow, imgWidth, imgHeight);

        ctx.globalAlpha = 1;
    }

    // draw the outline of a hexagon
    function strokeHexagon(x, y) {
        ctx.beginPath();
        ctx.moveTo(x + 0.5 * sideLength, y + hexRectangleHeight);
        ctx.lineTo(x + 1.5 * sideLength, y + hexRectangleHeight);
        ctx.lineTo(x + hexRectangleWidth, y + hexRectangleHeight + hexInnerRadius);
        ctx.lineTo(x + 1.5 * sideLength, y + hexRectangleHeight + hexRectangleHeight);
        ctx.lineTo(x + 0.5 * sideLength, y + hexRectangleHeight + hexRectangleHeight);
        ctx.lineTo(x, y + hexRectangleHeight + hexInnerRadius);
        ctx.closePath();
        ctx.stroke();
    }

    // convert offset coordinates to cube coordinates
    function offsetToCubeCoords(col, row) {
        var x = col;
        var z = row - (col - (col & 1)) / 2;
        var y = -x - z;
        return [x, y, z];
    }

    // convert cube coordinates to axial coordinates
    function cubeToAxialCoords(x, y, z) {
        var q = x;
        var r = z;
        return [q, r];
    }

    // convert offset coordinates to axial coordinates
    function offsetToAxialCoords(col, row) {
        var cube = offsetToCubeCoords(col, row);
        var axial = cubeToAxialCoords(cube[0], cube[1], cube[2]);
        return [axial[0], axial[1]];
    }

    // convert axial coordinates to a pixel coordinate in the middle of the hexagon
    function axialToPixelCoords(q, r) {
        var x = sideLength * (3. / 2 * q);
        var y = sideLength * (Math.sqrt(3) / 2 * q + Math.sqrt(3) * r);

        //set pixel coordinates to the middle of the hexagon
        x = Math.round(x + sideLength);
        y = Math.round(y + hexInnerRadius);
        return [x, y];
    }

    // convert offset coordinates to a pixel coordinate in the middle of the hexagon
    function offsetToPixelCoords(col, row) {
        var axial = offsetToAxialCoords(col, row);
        var pixel = axialToPixelCoords(axial[0], axial[1]);
        return [pixel[0], pixel[1]];
    }

    // get the coordinates of a neighbor of a hexagon depending on the direction
    function getNeighborCoords(col, row, direction) {
        var parity = col & 1;
        var dir = offsetDirections[parity][direction];
        return [col + dir[0], row + dir[1]];
    }
    
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