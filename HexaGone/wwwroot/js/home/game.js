var frame;
var lastTick = performance.now();
let controller;

$(function () {
    // setting the game variables from the Game.cshtml file
    gameVariables();
    controller = new Controller();
    Controller.enableEvents(controller);

    if (canvas.getContext) {

        // ensure that the textures stay high-quality when zooming in on the map
        ctx.imageSmoothingEnabled = false;

        // set the style when drawing figures inside of the canvas
        ctx.fillStyle = "#CCCCCC";
        ctx.strokeStyle = "#000000";
        ctx.lineWidth = 4;
    }
    requestAnimationFrame(render);
});

function render(currentTick) {
    var delta = currentTick - lastTick
    lastTick = currentTick;

    draw();

    frame = requestAnimationFrame(render);
}

function draw() {
    clearMap();

    //ctx.setTransform(scaleX, 0, 0, scaleY,
    //    offsetX, offsetY);

    drawMap();
}

function highlightTileAtMousePosition() {
    // Position of the mouse cursor on the canvas
    var mousePosX;
    var mousePosY;

    // offset-coordinate of the current tile the mouse is hovering over
    var hexX;
    var hexY;

    //

    mousePosX = controller.mousePosition.x / controller.scale + controller.origin.x;
    mousePosY = controller.mousePosition.y / controller.scale + controller.origin.y;

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
}

// clear the canvas
function clearMap() {

    ctx.clearRect(controller.origin.x, controller.origin.y, controller.visibleWidth, controller.visibleWidth);
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

//var isDown = false;
//var lastMousePos = { x: 0, y: 0 };
//var deltaMousePos = { x: 0, y: 0 };

//canvas.onmousedown = function (e) {
//    isDown = true;
//    lastMousePos.x = e.offsetX;
//    lastMousePos.y = e.offsetY;
//}

//canvas.onmouseup = function (e) {
//    isDown = false;
//}

//canvas.onmousemove = function (e) {
//    if (!isDown) return;

//    deltaMousePos.x = e.offsetX - lastMousePos.x;
//    deltaMousePos.y = e.offsetY - lastMousePos.y;

//    //ctx.translate(deltaMousePos.x, deltaMousePos.y);

//    offsetX += deltaMousePos.x;
//    offsetY += deltaMousePos.y;

//    lastMousePos.x = e.offsetX;
//    lastMousePos.y = e.offsetY;
//}

class Controller {
    constructor() {
        this.width = canvas.width;
        this.height = canvas.height;
        this.visibleWidth = this.width;
        this.visibleHeight = this.height;

        this.origin = { x: 0, y: 0 };
        this.oldMousePosition = { x: 0, y: 0 };
        this.mousePosition = { x: 0, y: 0 };
        this.dragDelta = { x: 0, y: 0 };
        this.mouseDown = false;

        this.scale = 1;
        this.wheel = 1;
        this.zoom = 1;
        this.zoomFactor = 0.2;
    }

    static enableEvents(_controller) {
        let c = $(canvas);
        c.mousedown(function (e) { _controller.updateMouseDown(e); });
        c.mouseup(function (e) { _controller.updateMouseUp(e); });
        c.mousemove(function (e) { _controller.updateMouseMove(e); });
        canvas.addEventListener('wheel', function (e) { _controller.updateMouseWheel(e); });
    }

    updateMouseDown(e) {
        this.mouseDown = true;
        this.oldMousePosition = { x: e.clientX, y: e.clientY };
    }

    updateMouseUp(e) {
        this.mouseDown = false;
    }

    updateMouseMove(e) {
        this.mousePosition.x = e.clientX - canvas.offsetLeft;
        this.mousePosition.y = e.clientY - canvas.offsetTop;

        highlightTileAtMousePosition();

        if (!this.mouseDown) return;

        this.dragDelta.x = this.mousePosition.x - this.oldMousePosition.x;
        this.dragDelta.y = this.mousePosition.y - this.oldMousePosition.y;

        this.oldMousePosition.x = this.mousePosition.x;
        this.oldMousePosition.y = this.mousePosition.y;

        ctx.translate(this.dragDelta.x / this.scale, this.dragDelta.y / this.scale);

        this.origin.x -= this.dragDelta.x / this.scale;
        this.origin.y -= this.dragDelta.y / this.scale;
    }

    updateMouseWheel(e) {
        e.preventDefault();

        // Get mouse offset.
        // this.mousePosition.x = e.clientX - canvas.offsetLeft;
        // this.mousePosition.y = e.clientY - canvas.offsetTop;
        // Normalize wheel to +1 or -1.
        this.wheel = e.deltaY < 0 ? 1 : -1;
        // Compute zoom factor.
        this.zoom = Math.exp(this.wheel * this.zoomFactor);
        // Translate so the visible origin is at the context's origin.
        ctx.translate(this.origin.x, this.origin.y);

        // Compute the new visible origin. Originally the mouse is at a
        // distance mouse/scale from the corner, we want the point under
        // the mouse to remain in the same place after the zoom, but this
        // is at mouse/new_scale away from the corner. Therefore we need to
        // shift the origin (coordinates of the corner) to account for this.
        this.origin.x -= this.mousePosition.x / (this.scale * this.zoom) - this.mousePosition.x / this.scale;
        this.origin.y -= this.mousePosition.y / (this.scale * this.zoom) - this.mousePosition.y / this.scale;

        // Scale it (centered around the origin due to the trasnslate above).
        ctx.scale(this.zoom, this.zoom);
        // Offset the visible origin to it's proper position.
        ctx.translate(-this.origin.x, -this.origin.y);

        // Update scale and others.
        this.scale *= this.zoom;
        this.visibleWidth = this.width / this.scale;
        this.visibleHeight = this.height / this.scale;
    }
}
