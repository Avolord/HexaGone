﻿(function () {
    game_variables();
    var off_directions = [
        [[+1, 0], [0, -1], [-1, -1],
        [-1, 0], [-1, +1], [0, +1]],
        [[+1, 0], [+1, -1], [0, -1],
        [-1, 0], [0, +1], [+1, +1]],
    ];

    //===
    // Test
    document.getElementById("test1").innerHTML = textures;
    //===

    hexHeight = Math.sin(hexagonAngle) * sideLength;
    hexRadius = Math.cos(hexagonAngle) * sideLength;
    hexRectangleHeight = sideLength + 2 * hexHeight;
    hexRectangleWidth = 2 * hexRadius;

    if (canvas.getContext) {
        var ctx = canvas.getContext('2d');

        ctx.fillStyle = "#000000";
        ctx.strokeStyle = "#CCCCCC";
        ctx.lineWidth = 3;

        drawBoard(ctx, boardWidth, boardHeight);

        canvas.addEventListener("mousemove", function (eventInfo) {
            var x,
                y,
                hexX,
                hexY,

                x = eventInfo.offsetX || eventInfo.layerX;
            y = eventInfo.offsetY || eventInfo.layerY;

            hexY = Math.floor(y / (hexHeight + sideLength));
            hexX = Math.floor((x - (hexY % 2) * hexRadius) / hexRectangleWidth);


            //===
            // Compare original Hexagon(hexX, hexY) with two neighbors.
            // The Hexagon with the shortest distance to the cursor is the correct one.

            var p = off_to_pixel(hexX, hexY);
            var d = distance(p[0], p[1], x, y);

            var n2 = off_neighbor(hexX, hexY, 2);
            var p2 = off_to_pixel(n2[0], n2[1]);
            var d2 = distance(p2[0], p2[1], x, y);

            var n1 = off_neighbor(hexX, hexY, 1);
            var p1 = off_to_pixel(n1[0], n1[1]);
            var d1 = distance(p1[0], p1[1], x, y)


            if (d2 < d && d2 < d1) {
                hexX = n2[0];
                hexY = n2[1];
            }
            else if (d1 < d) {
                hexX = n1[0];
                hexY = n1[1];
            }
            //===

            ctx.clearRect(0, 0, canvas.width, canvas.height);
            drawBoard(ctx, boardWidth, boardHeight);

            // Check if the mouse's coords are on the board
            if (hexX >= 0 && hexX < boardWidth) {
                if (hexY >= 0 && hexY < boardHeight) {
                    ctx.fillStyle = "#000000";
                    drawHexagon(ctx, hexX, hexY, true);
                }
            }


        });
    }

    function drawBoard(canvasContext, width, height) {
        var i,
            j;

        for (i = 0; i < width; ++i) {
            for (j = 0; j < height; ++j) {
                drawHexagon(
                    ctx,
                    i,
                    j,
                    false
                );
            }
        }
    }

    function drawHexagon(canvasContext, x, y, fill) {
        var fill = fill || false;

        x = x * hexRectangleWidth + ((y % 2) * hexRadius);
        y = y * (sideLength + hexHeight);

        canvasContext.beginPath();
        canvasContext.moveTo(x + hexRadius, y);
        canvasContext.lineTo(x + hexRectangleWidth, y + hexHeight);
        canvasContext.lineTo(x + hexRectangleWidth, y + hexHeight + sideLength);
        canvasContext.lineTo(x + hexRadius, y + hexRectangleHeight);
        canvasContext.lineTo(x, y + sideLength + hexHeight);
        canvasContext.lineTo(x, y + hexHeight);
        canvasContext.closePath();

        if (fill) {
            canvasContext.fill();
        } else {
            canvasContext.stroke();
        }
    }

    function pixel_to_axial(x, y) {
        // set pixel coordinates to the middle of the hexagon
        //x = x + hexRadius;
        //y = y + sideLength;

        var q = (Math.sqrt(3) / 3 * x - 1 / 3 * y) / sideLength;
        var r = (2. / 3 * y) / sideLength;

        var axial = axial_round(q, r)

        return [axial[0], axial[1]];
    }

    function axial_to_pixel(q, r) {
        var x = sideLength * (Math.sqrt(3) * q + Math.sqrt(3) / 2 * r);
        var y = sideLength * (3. / 2 * r);

        // set pixel coordinates to the middle of the hexagon
        x = Math.round(x + hexRadius);
        y = Math.round(y + sideLength);

        return [x, y];
    }

    function off_to_pixel(x, y) {
        var axial = off_to_axial(x, y);
        var pixel = axial_to_pixel(axial[0], axial[1]);
        return [pixel[0], pixel[1]];
    }

    function cube_round(x, y, z) {
        var rx = Math.round(x);
        var ry = Math.round(y);
        var rz = Math.round(z);

        var x_diff = Math.abs(rx - x);
        var y_diff = Math.abs(ry - y);
        var z_diff = Math.abs(rz - z);

        if (x_diff > y_diff && x_diff > z_diff) {
            rx = -ry - rz;
        }
        else if (y_diff > z_diff) {
            ry = -rx - rz;
        }
        else {
            rz = -rx - ry;
        }

        return [rx, ry, rz];
    }

    function axial_round(x, y) {

        var cube = axial_to_cube(x, y);
        var rCube = cube_round(cube[0], cube[1], cube[2]);
        var axial = cube_to_axial(rCube[0], rCube[1], rCube[2])

        return [axial[0], axial[1]];
    }

    function axial_to_cube(x, z) {

        var y = -x - z;

        return [x, y, z];
    }

    function cube_to_axial(x, y, z) {

        return [x, z];
    }

    function off_to_cube(col, row) {
        var x = col - (row - (row & 1)) / 2;
        var z = row;
        var y = -x - z;
        return [x, y, z];
    }

    function cube_to_off(x, y, z) {
        var col = x + (z - (z & 1)) / 2;
        var row = z;
        return [col, row];
    }

    function off_to_axial(row, col) {
        var cube = off_to_cube(row, col);
        var axial = cube_to_axial(cube[0], cube[1], cube[2])
        return [axial[0], axial[1]];
    }

    function axial_to_off(p, r) {
        var cube = axial_to_cube(p, r);
        var off = cube_to_off(cube[0], cube[1], cube[2]);
        return [off[0], off[1]];
    }

    function distance(x1, y1, x2, y2) {
        return Math.round(Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2)))
    }

    function axial_neighbor(p, r, direction) {
        var dir = axial_directions[direction];
        return [p + dir[0], r + dir[1]];
    }

    function off_neighbor(col, row, direction) {
        var parity = row & 1;
        var dir = off_directions[parity][direction];
        return [col + dir[0], row + dir[1]];
    }
})();