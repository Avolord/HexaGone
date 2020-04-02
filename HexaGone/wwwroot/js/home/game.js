(function () {

    gameVariables();

    //===
    // Test
    //document.getElementById("test1").innerHTML = img.naturalHeight;
    //===

    if (hexmapStyle == "pointy") {
        gameVariables_p();

        if (canvas.getContext) {
            var ctx = canvas.getContext('2d');

            ctx.fillStyle = "#CCCCCC";
            ctx.strokeStyle = "#000000";
            ctx.lineWidth = 3;

            drawBoard_p(ctx, boardWidth, boardHeight);


            canvas.addEventListener("mousemove", function (eventInfo) {
                var x,
                    y,
                    hexX,
                    hexY;

                x = eventInfo.offsetX || eventInfo.layerX;
                y = eventInfo.offsetY || eventInfo.layerY;

                hexY = Math.floor(y / (hexHeight + sideLength));
                hexX = Math.floor((x - (hexY % 2) * hexRadius) / hexRectangleWidth);

                
                //===
                // Compare original Hexagon(hexX, hexY) with two neighbors.
                // The Hexagon with the shortest distance to the cursor is the correct one.
                var p = offToPixel_p(hexX, hexY);
                var d = distance(p[0], p[1], x, y);

                var n2 = offNeighbor_p(hexX, hexY, 2);
                var p2 = offToPixel_p(n2[0], n2[1]);
                var d2 = distance(p2[0], p2[1], x, y);

                var n1 = offNeighbor_p(hexX, hexY, 1);
                var p1 = offToPixel_p(n1[0], n1[1]);
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
                drawBoard_p(ctx, boardWidth, boardHeight);

                // Check if the mouse's coords are on the board
                if (hexX >= 0 && hexX < boardWidth) {
                    if (hexY >= 0 && hexY < boardHeight) {
                        ctx.fillStyle = "#ffffff";
                        ctx.globalAlpha = 0.5;
                        drawHexagon_p(ctx, hexX, hexY, true);
                        ctx.globalAlpha = 1.0;
                    }
                }


            });
        }
    }
    else if (hexmapStyle == "flat") {
        gameVariables_f();

        if (canvas.getContext) {
            var ctx = canvas.getContext('2d');

            ctx.fillStyle = "#CCCCCC";
            ctx.strokeStyle = "#ffffff";
            ctx.lineWidth = 2;

            //drawBoard_f(ctx, boardWidth, boardHeight);
            drawTextureBoard(ctx, img, boardWidth, boardHeight);

            canvas.addEventListener("mousemove", function (eventInfo) {
                var x,
                    y,
                    hexX,
                    hexY;

                x = eventInfo.offsetX || eventInfo.layerX;
                y = eventInfo.offsetY || eventInfo.layerY;

                hexX = Math.floor(x / (hexHeight + sideLength));
                hexY = Math.floor((y - (hexX % 2) * hexRadius) / hexRectangleHeight);

                ctx.clearRect(0, 0, canvas.width, canvas.height);
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


                document.getElementById("test1").innerHTML = x;
                document.getElementById("test2").innerHTML = y;
                document.getElementById("test3").innerHTML = "test";
                document.getElementById("test4").innerHTML = "test";


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
                if (hexX >= 0 && hexX < boardWidth) {
                    if (hexY >= 0 && hexY < boardHeight) {
                        ctx.fillStyle = "#ffffff";
                        ctx.globalAlpha = 0.5;
                        drawHexagon_f(ctx, hexX, hexY, false);
                        ctx.globalAlpha = 1.0;
                    }
                }
            });
        }
    }

    
    
    
    


    //=====
    // Functions:
    function distance(x1, y1, x2, y2) {
        return Math.round(Math.sqrt(Math.pow(x2 - x1, 2) + Math.pow(y2 - y1, 2)))
    }

    function drawTexture(canvasContext, image, col, row, textureIndex) {
        pixelCol = col * (sideLength + hexHeight)- col * (0 * sideLength);
        pixelRow = (row - 1) * hexRectangleHeight + ((col % 2) * hexRadius) - row * (0.18 * sideLength);

        var imgHeight = hexRectangleHeight * 2;
        var imgWidth = hexRectangleWidth;
        var tileX = textureIndex * 128;
        
        canvasContext.drawImage(image, tileX, 0, 128, 248, pixelCol, pixelRow, imgWidth, imgHeight);
    }

    function drawTextureBoard(canvasContext, image, width, height) {
        for (var i = 0; i < height; i++) {
            for (var j = 0; j < width; j += 2) {
                drawTexture(canvasContext, image, j, i, textures[j][i]);
            }
            for (var j = 1; j < width; j += 2) {
                drawTexture(canvasContext, image, j, i, textures[j][i]);
            }
        }
    }
    //=====


    //=====
    // Functions for pointy top hexagons:
    function drawBoard_p(canvasContext, width, height) {
        var i,
            j;

        for (i = 0; i < width; ++i) {
            for (j = 0; j < height; ++j) {
                drawColoredHex_p(canvasContext, i, j, textures[i][j]);
            }
        }
    }

    function drawColoredHex_p(canvasContext, x, y, colorIndex) {
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
        drawHexagon_p(canvasContext, x, y, true);
    }
    
    function drawHexagon_p(canvasContext, x, y, fill) {
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

    function axialToPixel_p(q, r) {
        var x = sideLength * (Math.sqrt(3) * q + Math.sqrt(3) / 2 * r);
        var y = sideLength * (3. / 2 * r);

        // set pixel coordinates to the middle of the hexagon
        x = Math.round(x + hexRadius);
        y = Math.round(y + sideLength);

        return [x, y];
    }

    function offToPixel_p(x, y) {
        var axial = offToAxial_p(x, y);
        var pixel = axialToPixel_p(axial[0], axial[1]);
        return [pixel[0], pixel[1]];
    }

    function cubeToAxial_p(x, y, z) {

        return [x, z];
    }

    function offToCube_p(col, row) {
        var x = col - (row - (row & 1)) / 2;
        var z = row;
        var y = -x - z;
        return [x, y, z];
    }

    function offToAxial_p(row, col) {
        var cube = offToCube_p(row, col);
        var axial = cubeToAxial_p(cube[0], cube[1], cube[2])
        return [axial[0], axial[1]];
    }

    function offNeighbor_p(col, row, direction) {
        var parity = row & 1;
        var dir = offDirections[parity][direction];
        return [col + dir[0], row + dir[1]];
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

        row = row * hexRectangleHeight + ((col % 2) * hexRadius) - row * (0.125 * sideLength);
        col = col * (sideLength + hexHeight) - col * (0.0625 * sideLength);
        

        canvasContext.beginPath();
        canvasContext.moveTo(col + hexHeight, row);
        canvasContext.lineTo(col + hexHeight + sideLength, row);
        canvasContext.lineTo(col + hexRectangleWidth, row + hexRadius);
        canvasContext.lineTo(col + hexHeight + sideLength, row + hexRectangleHeight);
        canvasContext.lineTo(col + hexHeight, row + hexRectangleHeight);
        canvasContext.lineTo(col, row + hexRadius);
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
        y = Math.round(y + hexRadius);
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