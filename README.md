### ⚠️ IMPORTANT NOTICE / DISCLAIMER

**Original Author:** warawanaidene (original), Soulztorm (Wara- fork this port descends from)
**Original Repository:** Modding Stats Helper
**Original Link:** https://github.com/Soulztorm/Wara-ModdingStatsHelper (from https://github.com/warawanaidene/ModdingStatsHelper)
**License:** See upstream repository
**This Port By:** R_F (danyhappy564-cmyk) — unofficial, AI-assisted port. Not affiliated with or endorsed by the original author.

1. **Reflection & Take-Downs:** I deeply reflect on the ECOT incident. As an AI-assisted "vibe coder," I will immediately delete files if the original authors ask.
2. **No Re-Distribution:** These ported builds are unverified, temporary fixes. Please do NOT re-upload or share them anywhere else.
3. **Do Not Pester Original Authors:** Never report bugs or pester original modders regarding issues from my unofficial ports.
4. **Full Credit & Respect:** I will always credit original creators on GitHub and prioritize their decisions above all else.
5. **Support Original Creators:** Instead of using my ports, please visit the original authors' Forge pages to leave kind words or tips.

---

# Modding Stats Helper (fork)

> **원작자 · 원본 레포**
> **wara** / ChooChoo — https://github.com/danyhappy564-cmyk/Wara-ModdingStatsHelper 의 원본
>
> 이 레포는 위 원작의 **포크**입니다. 기능은 그대로고, **SPT 4.1에서 빌드·동작하도록
> 포팅**한 것이 전부입니다.

무기 개조 화면에서 부품에 마우스를 올리면 툴팁에 스탯을 붙여 보여주고, 현재 장착된
부품과 **비교**해서 증감을 색으로 표시합니다. `CTRL`로 스탯 보기 ↔ 비교 보기를 전환합니다.

현재 기준 **SPT 4.1**.

---

## 4.1 포팅에서 바뀐 것

**타입 이름 하나:** `ItemAttributeClass` → `EFT.InventoryLogic.ItemAttribute`

4.1이 클라이언트를 역난독화하면서 바뀐 이름입니다. 독립된 두 출처로 교차 확인했습니다:

| 출처 | 결과 |
|---|---|
| SPT 4.1 wiki `Class_Name_Mappings.md` | `` `ItemAttributeClass` `` → `` `EFT.InventoryLogic.ItemAttribute` `` |
| assembly-tool `GClass-Mappings.json5` | `GClass3375` → `ItemAttribute` (namespace `EFT.InventoryLogic`) |

나머지 타입(`SimpleTooltip`, `GridItemView`, `DropDownMenu`, `ModdingScreenSlotView`,
`EditBuildScreen`, `MenuTaskBar`, `EEftScreenType`, `EItemAttributeId`,
`EItemAttributeLabelVariations`)은 원래부터 실명이라 리네임 표에 없습니다 = 그대로입니다.

**`slot_0` 필드는 안 바뀝니다.** assembly-tool의 `ObfuscatedFieldRenamer`는 이름이
난독화 접두사(`Class`, `GClass`, `GStruct`, `method` …)로 시작하는 필드만 건드리는데
`slot`은 목록에 없습니다. 다만 그건 **도구의 성질**이지 BSG의 약속은 아니라서, 조회를
방어적으로 바꿔뒀습니다 — `slot_0`을 먼저 찾고, 없으면 `ModdingScreenSlotView`에서 유일한
`Slot` 타입 인스턴스 필드로 대체하며 로그를 남깁니다. 예전에는 못 찾으면 `null` 체크만
하고 조용히 넘어가서, 비교 기능이 죽어도 로그에 아무 말이 없었습니다.

**빌드 설정도 갈아엎었습니다:**

- 구식 csproj → SDK 스타일, `net471` → `netstandard2.1`
- `..\..\..\` 상대 경로 전부 제거. 이 경로는 레포가 SPT 폴더 **안에 정확히 3단계**로
  들어가 있어야만 풀립니다. 다른 데 클론하면 모든 참조가 조용히 미해결이 됐습니다
- `spt-core` 참조 삭제 — 코드에서 `SPT.Core` 타입을 하나도 안 쓰고, 4.1 설치본에는 그
  어셈블리가 없습니다
- `Properties/AssemblyInfo.cs` 삭제, 값은 csproj로 이전 (버전 `1.1.1` 유지)

---

## 빌드

```
dotnet build Wara-ModdingStatsHelper.sln
```

경로는 `SptRoot`에서 나옵니다. 기본값 `E:\SPT 4.1`, `-p:SptRoot=...` 또는 동명의
환경변수로 덮어쓸 수 있습니다. 경로가 틀리면 참조 오류가 쏟아지는 대신 이유를 말해줍니다:

```
error : SptRoot 'D:\wrong' does not look like an SPT install
        (missing EscapeFromTarkov_Data\Managed\Assembly-CSharp.dll).
```

빌드하면 `$(SptRoot)\BepInEx\plugins\`로 바로 복사되고, `Release\`에 배포용 zip도 생깁니다.

## 설치

`Wara-ModdingStatsHelper.dll`을 `BepInEx\plugins\`에 넣으면 끝입니다. 서버 쪽 구성요소는
없습니다.
