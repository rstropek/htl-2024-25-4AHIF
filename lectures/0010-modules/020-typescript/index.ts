const WIDTH = 10;
const HEIGHT = 10;
const MINES = 10;
const CELL_CLASS = 'cell';
const OPENED_CLASS = 'opened';
const FLAG_SYMBOL = '🚩';
const MINE_SYMBOL = '💥';

const app = document.getElementById('app') as HTMLDivElement;
const message = document.getElementById('message') as HTMLParagraphElement;
const cells: HTMLDivElement[][] = [];

app.style.gridTemplateColumns = `repeat(${WIDTH}, auto)`;
for (let y = 0; y < HEIGHT; y++) {
  const row: HTMLDivElement[] = [];
  for (let x = 0; x < WIDTH; x++) {
    const cell = document.createElement('div');
    row.push(cell);
    cell.classList.add(CELL_CLASS);
    cell.addEventListener('click', () => handleCellClick(x, y));
    cell.addEventListener('contextmenu', (e) => {
      e.preventDefault();
      handleRightClick(x, y);
    });
    app.appendChild(cell);
  }

  cells.push(row);
}

let mines: { x: number; y: number }[] | undefined;
let gameOver = false;

function handleCellClick(x: number, y: number): void {
  const cell = cells[y][x];

  if (gameOver || cell.innerText === FLAG_SYMBOL || cell.classList.contains(OPENED_CLASS)) {
    return;
  }

  if (!mines) {
    mines = [];
    for (let i = 0; i < MINES; i++) {
      let rx: number, ry: number;
      do {
        rx = Math.floor(Math.random() * WIDTH);
        ry = Math.floor(Math.random() * HEIGHT);
      } while ((rx === x && ry === y) || mines.some((mine) => mine.x === rx && mine.y === ry));
      mines.push({ x: rx, y: ry });
    }
  }

  if (mines.some((mine) => mine.x === x && mine.y === y)) {
    cell.innerText = MINE_SYMBOL;
    gameOver = true;
    message.innerText = 'Game over!';
    message.style.display = 'block';
    return;
  }

  openCell(x, y);
}

function openCell(x: number, y: number): void {
  if (x < 0 || x >= WIDTH || y < 0 || y >= HEIGHT) {
    return;
  }

  const cell = cells[y][x];
  if (cell.classList.contains(OPENED_CLASS)) {
    return;
  }

  cell.classList.add(OPENED_CLASS);
  const minesAround = mines!.filter((mine) => Math.abs(mine.x - x) <= 1 && Math.abs(mine.y - y) <= 1).length;
  if (minesAround > 0) {
    cell.innerText = minesAround.toString();
    return;
  }

  const directions = [-1, 0, 1];
  directions.forEach((dx) => {
    directions.forEach((dy) => {
      if (dx !== 0 || dy !== 0) {
        openCell(x + dx, y + dy);
      }
    });
  });
}

function handleRightClick(x: number, y: number): void {
  const cell = cells[y][x];

  if (gameOver || cell.classList.contains(OPENED_CLASS) || !mines) {
    return;
  }

  cell.innerText = cell.innerText === FLAG_SYMBOL ? '' : FLAG_SYMBOL;

  if (mines.every((mine) => cells[mine.y][mine.x].innerText === FLAG_SYMBOL)) {
    message.innerText = 'You won!';
    message.style.display = 'block';
    gameOver = true;
  }
}
