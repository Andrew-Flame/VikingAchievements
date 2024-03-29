﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VikingAchievements.UI.Tab;

namespace VikingAchievements.UI.Icon; 

internal sealed class AchieveIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private static readonly Color DefaultColor = new(1f, 0.7176f, 0.3603f), 
                                  HoveredColor = new (1f, 1f, 1f, 0.9f), 
                                  FocusedColor = new (1f, 1f, 1f, 0.2f);
    
    public Image image;

    public void OnPointerEnter(PointerEventData eventData) {
        if (!TabManager.isOpened) SetHovered();
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!TabManager.isOpened) SetDefault();
    }

    public void SetDefault() => image.color = DefaultColor;
    
    public void SetHovered() => image.color = HoveredColor;

    public void SetFocused() => image.color = FocusedColor;
}