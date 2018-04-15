using System;

public class  spring2018_2 {

static public void Main() {

}
//  細線化（Hilditch）
private void ProcHilditch()
{
  // 近傍画素へのオフセット値
  Point[] p = {new Point(0, 0),
               new Point(0, 1),
               new Point(-1, 1),
               new Point(-1, 0),
               new Point(-1, -1),
               new Point(0, -1),
               new Point(1, -1),
               new Point(1, 0),
               new Point(1, 1)};
  
  // 近傍番号(奇数)
  int[] n = { 1, 3, 5, 7 };
  
  int x1, y1;           // 近傍画素のX、Y絶対座標
  int[] b = new int[9]; // 着目座標を含む9近傍の値
  int cnt = 0;          // ラベルに変化が生じた画素の数
  int sum;              // 制御変数等
  
  // 元画像データの設定
  int[,] src = new int[bs.Width, bs.Height];
  for (int y = 0; y < bs.Height; y++)
  {
    for (int x = 0; x < bs.Width; x++)
    {
      if (bs.GetPixel(x, y).GetBrightness() < 0.5) src[x, y] = 0;
      else src[x, y] = 1;
    }
  }
  
  // 配列の初期化
  int[,] dst = new int[bs.Width, bs.Height];
  for (int y = 0; y < bs.Height; y++)
  {
    for (int x = 0; x < bs.Width; x++)
    {
      dst[x, y] = src[x, y];
    }
  }
  
  // 細線化処理
  do
  {
    cnt = 0;
    for (int y = 0; y < bs.Height; y++)
    {
      for (int x = 0; x < bs.Width; x++)
      {
        // 座標(X,Y)を含む9近傍のデータ取得
        for (int i = 0; i < 9; i++)
        {
          b[i] = 0;
          x1 = x + p[i].X;
          y1 = y + p[i].Y;
          if ((x1 >= 0 && x1 < bs.Width) && (y1 >= 0 && y1 < bs.Height))
          {
            if (dst[x1, y1] == 1) b[i] = 1;
            else if (dst[x1, y1] == -1) b[i] = -1;
          }
        }
        // 条件1：図形画素で有る
        if (b[0] != 1) continue;
        // 条件2：境界点で有る
        sum = 0;
        for (int i = 0; i < 4; i++) sum = sum + 1 - Math.Abs(b[n[i]]);
        if (sum < 1) continue;
        // 条件3：端点を除去しない
        sum = 0;
        for (int i = 1; i < 9; i++) sum = sum + Math.Abs(b[i]);
        if (sum < 2) continue;
        // 条件4：孤立点を保存する
        sum = 0;
        for (int i = 1; i < 9; i++)
        {
          if (b[i] == 1) sum++;
        }
        if (sum < 1) continue;
        // 条件5：連結性を保存
        if (ProcHilditchSub(b) != 1) continue;
        // 条件6：線幅2の線分の片側だけを削除する
        sum = 0;
        for (int i = 1; i < 9; i++)
        {
          if (b[i] != -1)
          {
            sum++;
          }
          else
          {
            int tmp = b[i];
            b[i] = 0;
            if (ProcHilditchSub(b) == 1) sum++;
            b[i] = tmp;
          }
        }
        if (sum != 8) continue;
        //  削除候補の設定（此の段階では削除不可）
        dst[x, y] = -1;
        cnt++;
      }
    }
    // 画素の削除
    if (cnt != 0)
    {
      for (int y = 0; y < bs.Height; y++)
      {
        for (int x = 0; x < bs.Width; x++)
        {
          if (dst[x, y] == -1) dst[x, y] = 0;
        }
      }
    }
  } while (cnt > 0);
  
  // 描画
  for (int y = 0; y < bs.Height; y++)
  {
    for (int x = 0; x < bs.Width; x++)
    {
      if (dst[x, y] == 0)
        bd.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0));
      else
        bd.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
    }
  }
  picDest.Refresh();
}

// 細線化サブ（Hilditch）
private int ProcHilditchSub(int[] b)
{
  int t = 0;
  int[] n = { 1, 3, 5, 7 };
  
  int sum;
  int[] d = new int[10];
  
  for (int i = 0; i < 10; i++)
  {
    t = i;
    if (i == 9) t = 1;
    if (Math.Abs(b[t]) == 1) d[i] = 1;
    else d[i] = 0;
  }
  sum = 0;
  for (int i = 0; i < 4; i++)
  {
    t = n[i];
    sum = sum + d[t] - (d[t] * d[t + 1] * d[t + 2]);
  }
  return sum;
}
}