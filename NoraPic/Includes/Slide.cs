using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NoraPic.Includes
{
    public class Slide
    {
        /**
        <Canvas>
            <Image x:Name="imgImage" Source="{Binding ...}" Width="..." Height="...">
                <Image.RenderTransform>
                    <CompositeTransform x:Name="imgImageTranslate" />
                </Image.RenderTransform>
            </Image>
        </Canvas>

        <local:CollectionFlow x:Name="ImageList" ItemTemplate="{StaticResource DataTemplate1}"
               ItemsPanel="{StaticResource ItemsPanelTemplate1}"/>
        <Image Source="Images/imgBack.png" Width="48" Height="48" HorizontalAlignment="Left"
               VerticalAlignment="Center" Margin="15,0,0,0" MouseLeftButtonDown="Left"/>
        <Image Source="Images/imgfORWARD.png" Width="48" Height="48" HorizontalAlignment="Right"
               VerticalAlignment="Center" Margin="0,0,15,0" MouseLeftButtonDown="Right" />      

    private void GestureListener_DragCompleted(object sender, DragCompletedGestureEventArgs e)
    {
        if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
        {
            var abs = Math.Abs(PANEL_DRAG_HORIZONTAL);
            if (abs > 75)
            {
                if (PANEL_DRAG_HORIZONTAL > 0) // MovePrevious;
                else //MoveNext();

                e.Handled = true;
            }
        }
    }


    double PANEL_DRAG_HORIZONTAL = 0;
    private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
    {
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                PANEL_DRAG_HORIZONTAL += e.HorizontalChange;

                var baseLeft = -imgImage.Width / 2;
                if (PANEL_DRAG_HORIZONTAL > 75) imgImageTranslate.OffsetX = baseLeft + PANEL_DRAG_HORIZONTAL;
                else if (PANEL_DRAG_HORIZONTAL < -75) imgImageTranslate.OffsetX = baseLeft + PANEL_DRAG_HORIZONTAL;
                else imgImageTranslate.OffsetX = baseLeft;
            }
        }
    }

    private void GestureListener_DragStarted(object sender, DragStartedGestureEventArgs e)
    {
        PANEL_DRAG_HORIZONTAL = 0;
    }
    **/
    }
}
