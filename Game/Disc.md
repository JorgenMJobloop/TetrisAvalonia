```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:controls="using:TetrisAvalonia.Controls"
        mc:Ignorable="d" d:DesignWidth="400" d:DesignHeight="300"
        Width="400" Height="600"
        x:Class="TetrisAvalonia.MainWindow"
        Background="#001"
        Title="TetrisAvalonia">
    <Grid ColumnDefinitions="100,200,100">
        <StackPanel Background="WhiteSmoke" HorizontalAlignment="Left">
            <TextBlock>
                Score
            </TextBlock>
        </StackPanel>

        <StackPanel Grid.Column="1">
            <Grid>
                <controls:GameCanvas x:Name="GameCanvas"
                                     Width="300"
                                     Height="500"/>
            </Grid>
        </StackPanel>

        <StackPanel Grid.Column="2" HorizontalAlignment="Right" Background="White">
            <TextBlock>
                Level
            </TextBlock>
        </StackPanel>
    </Grid>
</Window>

```